using System.Collections.Generic;

public class Npc : Sprite
{
    public List<string> Dialogo { get; set; }
    public int IndiceDialogo { get; set; }
    public bool Hablando { get; set; }

    public Npc(string nombre)
        : base(nombre)
    {
        width = 34;
        height = 48;
        Dialogo = new List<string>();
        IndiceDialogo = 0;
        Hablando = false;
    }
    //Serializar TO DO
}
