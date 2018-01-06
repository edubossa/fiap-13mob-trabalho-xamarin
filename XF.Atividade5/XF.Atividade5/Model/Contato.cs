using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XF.Atividade5.Model
{
    
    public class Contato
    {
        public Contato() {
            Telefones = new List<Telefone>();
        }

        public string Nome { get; set; }
        public string DisplayName { get; set; }
        public string SobreNome { get; set; }
        public List<Telefone> Telefones { get; set; }
        public string Numero {
            get {
                if (Telefones != null) {
                    return Telefones.FirstOrDefault().Numero;
                }                    
                return string.Empty;
            }
        }

        public ImageSource Foto { get; set; }

    }
}
