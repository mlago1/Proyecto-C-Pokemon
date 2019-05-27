using System.Collections.Generic;

public class Npc : Sprite
{
    public List<string> Dialogo { get; set; }
    public int IndiceDialogo { get; set; }
    public bool Hablando { get; set; }

    public Npc(string[][] direcciones)
    {
        width = 34;
        height = 48;
        Dialogo = new List<string>();
        IndiceDialogo = 0;
        Hablando = false;
        LoadSequence(RIGHT, direcciones[0]);
        LoadSequence(LEFT, direcciones[1]);
        LoadSequence(UP, direcciones[2]);
        LoadSequence(DOWN, direcciones[3]);
    }
}
