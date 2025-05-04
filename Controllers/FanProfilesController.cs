using Microsoft.AspNetCore.Mvc;
using FuriaPulseWeb.Models;
using System.Diagnostics;
using System.Text.Json;

namespace FuriaPulseWeb.Controllers
{
    public class FanProfilesController : Controller
    {
        // Lista estática para armazenar os perfis dos fãs em memória
        private static List<FanProfile> fans = new();

        // Exibe a lista de fãs cadastrados
        public IActionResult Index()
        {
            return View(fans);
        }

        // Exibe o formulário para criação de um novo perfil de fã
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Processa os dados enviados pelo formulário de criação
        [HttpPost]
        public async Task<IActionResult> Create(FanProfile fan)
        {
            string caminhoArquivo = string.Empty;

            // Upload do documento de identidade
            if (fan.DocumentoIdentidade != null && fan.DocumentoIdentidade.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                caminhoArquivo = Path.Combine(uploadsFolder, fan.DocumentoIdentidade.FileName);

                using (var stream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    await fan.DocumentoIdentidade.CopyToAsync(stream);
                }

                // Salva o nome do arquivo para exibição futura
                fan.DocumentoNome = fan.DocumentoIdentidade.FileName;
            }

            // Etapa de OCR para extrair texto do documento enviado
            if (!string.IsNullOrEmpty(caminhoArquivo) && System.IO.File.Exists(caminhoArquivo))
            {
                Console.WriteLine("CAMINHO DO ARQUIVO ENVIADO: " + caminhoArquivo);

                var resultadoOCR = ExecutarOCR(caminhoArquivo);

                if (!string.IsNullOrWhiteSpace(resultadoOCR))
                {
                    try
                    {
                        // Desserializa o JSON retornado pelo OCR (lista de strings extraídas)
                        var textoExtraido = JsonSerializer.Deserialize<List<string>>(resultadoOCR);

                        // Verifica se o nome e CPF constam no texto extraído
                        bool nomeValido = textoExtraido.Any(t => t.Contains(fan.Nome, StringComparison.OrdinalIgnoreCase));
                        bool cpfValido = textoExtraido.Any(t => t.Contains(fan.CPF));

                        if (nomeValido && cpfValido)
                        {
                            TempData["Msg"] = "✅ Documento validado com sucesso!";
                        }
                        else
                        {
                            TempData["Msg"] = "⚠️ Documento não confere com os dados informados.";
                        }
                    }
                    catch
                    {
                        TempData["Msg"] = "❌ Erro ao processar o documento.";
                    }
                }
                else
                {
                    TempData["Msg"] = "❌ O documento não retornou dados.";
                }
            }
            else
            {
                TempData["Msg"] = "⚠️ Nenhum documento foi enviado para validação.";
            }

            // Validação do link de perfil eSports via script Python externo
            if (!string.IsNullOrWhiteSpace(fan.LinkPerfilEsports))
            {
                string resultado = ValidarPerfilEsports(fan.LinkPerfilEsports);
                fan.DetalheValidacaoEsports = resultado;
                fan.PerfilEsportsValido = resultado.Trim().Equals("ESPORTS_OK", StringComparison.OrdinalIgnoreCase);
            }

            // Verifica se o perfil no Instagram possui menções à FURIA
            if (!string.IsNullOrWhiteSpace(fan.InstagramLink))
            {
                fan.ResultadoValidacaoFuria = ExecutarValidadorFuria(fan.InstagramLink);
            }

            // Adiciona o perfil à lista em memória
            fans.Add(fan);

            // Redireciona para a tela de listagem
            return RedirectToAction("Index");
        }

        // Executa script Python para realizar OCR (extração de texto do documento)
        private string ExecutarOCR(string caminhoArquivo)
        {
            string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "python", "ocr_validador.py");

            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{scriptPath}\" \"{caminhoArquivo}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                string erro = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine("OCR >>> OUTPUT:");
                Console.WriteLine(output);

                if (!string.IsNullOrWhiteSpace(erro))
                {
                    Console.WriteLine("OCR >>> ERRO:");
                    Console.WriteLine(erro);
                }

                // Retorna apenas a linha com o JSON (espera-se que o script retorne uma lista de strings)
                string linhaComJson = output.Split('\n').FirstOrDefault(l => l.TrimStart().StartsWith("["));
                return linhaComJson?.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEÇÃO AO EXECUTAR OCR:");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        // Executa script Python que valida o link de perfil de eSports
        private string ValidarPerfilEsports(string link)
        {
            var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "python", "ValidarEsports.py");

            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{scriptPath}\" \"{link}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine("ESPORTS >>> OUTPUT:");
                Console.WriteLine(output);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine("ESPORTS >>> ERRO:");
                    Console.WriteLine(error);
                }

                return output.Trim();
            }
            catch (Exception ex)
            {
                return $"Erro ao validar perfil: {ex.Message}";
            }
        }

        // Executa script Python que verifica menções à FURIA no perfil do Instagram
        private string ExecutarValidadorFuria(string instagramLink)
        {
            var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "python", "VerificaSeApoiaFuria.py");

            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{scriptPath}\" \"{instagramLink}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine("FURIA >>> OUTPUT:");
                Console.WriteLine(output);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine("FURIA >>> ERRO:");
                    Console.WriteLine(error);
                }

                return output.Trim();
            }
            catch (Exception ex)
            {
                return $"Erro: {ex.Message}";
            }
        }
    }
}
