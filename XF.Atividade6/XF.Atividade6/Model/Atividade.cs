using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace XF.Atividade6.Model
{
    public class Atividade
    {
        public string token { get; set; }
        public string dataCadastro { get; set; }
        public string dataEntrega { get; set; }
        public string tipoAvaliacao { get; set; }
        public string descricao { get; set; }
        public int valorAtividade { get; set; }
    }

    public static class AtividadeRepository
    {
        private static IEnumerable<Atividade> alunosAPIAzure;

        public static async Task<ObservableCollection<Atividade>> GetAtividades()
        {
            var httpRequest = new HttpClient();
            var stream = await httpRequest.GetStreamAsync("https://ewsfiapmob13.azurewebsites.net/api/lancamento_atividades");
            var atividadeSerializer = new DataContractJsonSerializer(typeof(List<Atividade>));
            alunosAPIAzure = (List<Atividade>)atividadeSerializer.ReadObject(stream);
            return new ObservableCollection<Atividade>(alunosAPIAzure);
        }

        public static async Task<bool> PostAtividade(Atividade atividadeAdd)
        {
            if (atividadeAdd == null)
            {
                return false;
            }
            var httpRequest = new HttpClient();
            httpRequest.BaseAddress = new Uri("https://ewsfiapmob13.azurewebsites.net/");
            httpRequest.DefaultRequestHeaders.Accept.Clear();
            httpRequest.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            string profJson = Newtonsoft.Json.JsonConvert.SerializeObject(atividadeAdd);
            var response = await httpRequest.PostAsync("api/lancamento_atividades", new StringContent(profJson, System.Text.Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public static async Task<bool> DeleteAtividade(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }
            var httpRequest = new HttpClient();
            httpRequest.BaseAddress = new Uri("https://ewsfiapmob13.azurewebsites.net/");
            httpRequest.DefaultRequestHeaders.Accept.Clear();
            httpRequest.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpRequest.DeleteAsync(string.Format("api/lancamento_atividades?token=" + token));
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }

}
