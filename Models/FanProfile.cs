using System.ComponentModel.DataAnnotations.Schema;

namespace FuriaPulseWeb.Models
{
    public class FanProfile
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }

        public string InstagramUser { get; set; }
        public string TwitterUser { get; set; }

        public string InstagramLink { get; set; } 
        public string TwitterLink { get; set; }

        public string Interesses { get; set; }
        public string Atividades { get; set; }
        public string ComprasRecentes { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public bool SegueFuria { get; set; } = false;

        public string RelacionamentoComEsports { get; set; }

        [NotMapped]
        public IFormFile DocumentoIdentidade { get; set; }

        public string DocumentoNome { get; set; }

        public string LinkPerfilEsports { get; set; }
        public bool PerfilEsportsValido { get; set; }
        public string DetalheValidacaoEsports { get; set; }

        public string ResultadoValidacaoFuria { get; set; } 
    }
}
