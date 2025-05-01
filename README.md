# FuriaPulse

🎮 Aplicação para cadastro e validação de fãs da FURIA Esports, com análise de identidade via OCR e validação de redes sociais com scripts em Python.

## 📸 Visão Geral

O sistema permite:

- Cadastro de fãs com dados pessoais e links de redes sociais.
- Upload de documentos de identidade com validação via OCR (EasyOCR).
- Verificação se o fã segue a FURIA no Instagram.
- Validação de perfis de e-sports (ex: FACEIT, Liquipedia etc).
- Interface responsiva com visual personalizado da FURIA.

## 🔧 Tecnologias Utilizadas

- ASP.NET Core MVC (.NET 8)
- Python 3.11
- EasyOCR + OpenCV
- Bootstrap 5
- HTML, CSS
- Scripts com `requests` e `BeautifulSoup`

---

## 📁 Estrutura de Pastas

FuriaPulseWeb/
├── Controllers/
│   └── FanProfilesController.cs

├── Models/
│   ├── FanProfile.cs
│   └── ErrorViewModels.cs

├── Views/
│   ├── FanProfiles/
│   │   ├── Create.cshtml
│   │   └── Index.cshtml
│   ├── Home/
│   │   └── Error.cshtml
│   └── Shared/
│       ├── _Layout.cshtml
│       ├── _ViewImports.cshtml
│       └── _ViewStart.cshtml

├── wwwroot/
│   ├── css/
│   │   └── site.css
│   ├── img/
│   │   ├── bg-furia.png
│   │   ├── bg-furia.svg
│   │   ├── check.png
│   │   └── cross.png
│   ├── js/
│   ├── lib/
│   ├── uploads/
│   └── favicon.ico

├── python/
│   ├── ocr_validador.py
│   ├── VerificaSeApoiaFuria.py
│   └── ValidarEsports.py
├── appsettings.json
├── Program.cs
└── launchSettings.json

---

## ⚙️ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Python 3.10+](https://www.python.org/downloads/)
- Git instalado

---
🧠 Como funciona a IA
O script ocr_validador.py usa EasyOCR para extrair texto da CNH enviada.

O script VerificaSeApoiaFuria.py verifica se o Instagram informado contém menção à FURIA.

O script ValidarEsports.py verifica se o link fornecido pertence a um site de e-sports reconhecido.


---
## 🚀 Como rodar o projeto localmente

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/FuriaPulse.git
cd FuriaPulseWeb
2. Instale os pacotes Python necessários
bash
Copiar
Editar
pip install easyocr opencv-python-headless requests beautifulsoup4
Se estiver no Windows e ocorrer erro com emojis ou caracteres especiais:

bash
Copiar
Editar
chcp 65001
🖼️ Imagens e Layout
O fundo da aplicação utiliza a imagem /wwwroot/img/bg-furia.png

Os ícones de validação estão em /wwwroot/img/check.png e /cross.png

▶️ Executando
Abra o projeto no Visual Studio (ou rode pelo terminal com dotnet run)

Acesse https://localhost:xxxx/

Faça upload de um documento, preencha redes sociais, e valide o perfil do fã

🤖 Scripts Python
Os scripts são executados via System.Diagnostics.Process no Controller e devem estar localizados em:

bash
Copiar
Editar
FuriaPulseWeb/python/
Certifique-se de que:

As permissões estão corretas

O caminho está correto no controller

💡 Sugestões
Conecte-se à API oficial da FURIA (se disponível) para verificar seguidores

Salve os cadastros em banco (EF Core ou SQLite)

📜 Licença
Projeto educacional sem fins lucrativos.
