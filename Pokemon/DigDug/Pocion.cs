public class Pocion : Objeto
{
    public int hpRecuperados { get; set; }

    public Pocion(string nombre, int hpRecuperados)
        :base(nombre)
    {
        this.hpRecuperados = hpRecuperados;
    }
}
