using System;
class Combate : Menu
{
    Jugador prota,rival;
    Image fondo_opciones;
    Bestia salvaje,seleccionado;
    bool turno;

    public Combate(Jugador prota, Bestia salvaje)
    {
        bg = new Image("data/fondo_combate.png");
        font24 = new Font("data/Joystix.ttf", 25);
        continuar = true;
        this.prota = prota;
        seleccionado = prota.GetEquipo()[0];
        this.salvaje = salvaje;
        salvaje.MoveTo(600,70);
        seleccionado.MoveTo(100,350);
        fondo_opciones = new Image("data/dialogo_combate.png");
    }

    public override void DibujarInterfaz()
    {
        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        salvaje.DrawOnHiddenScreen();
        DibujarInterfazSalvaje();
        DibujarInterfazSeleccionado(seleccionado);
        seleccionado.DrawOnHiddenScreen();
        SdlHardware.DrawHiddenImage(fondo_opciones,0,500);
        SdlHardware.ShowHiddenScreen();
    }

    private void DibujarInterfazSalvaje()
    {
        SdlHardware.WriteHiddenText(salvaje.GetNombre(),
                        100, 100,
                        0, 0, 0,
                        font24);
        SdlHardware.WriteHiddenText("Lvl: " + salvaje.GetNivel().ToString(),
                        100, 130,
                        0, 0, 0,
                        font24);
        SdlHardware.WriteHiddenText("HP: " + salvaje.GetVida().ToString(),
                        100, 160,
                        0, 0, 0,
                        font24);
    }

    private void DibujarInterfazSeleccionado(Bestia seleccionado)
    {
        SdlHardware.WriteHiddenText(seleccionado.GetNombre(),
                        650, 400,
                        0, 0, 0,
                        font24);
        SdlHardware.WriteHiddenText("Lvl: " + seleccionado.GetNivel().ToString(),
                        650, 430,
                        0, 0, 0,
                        font24);
        SdlHardware.WriteHiddenText("HP: " + seleccionado.GetVida().ToString(),
                        650, 460,
                        0, 0, 0,
                        font24);
    }

    public void Run()
    {
        do
        {
            DibujarInterfaz();
            if (SdlHardware.KeyPressed(SdlHardware.KEY_K)) //Provisional
            {
                continuar = false;
                SdlHardware.Pause(60);
            }
            SdlHardware.Pause(40);
        } while (continuar);
    }
}
