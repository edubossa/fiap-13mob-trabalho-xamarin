namespace XF.Atividade5.App_Code
{
    public interface ILocalizacao
    {
        void GetCoordenada();
    }
    public class Coordenada {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
