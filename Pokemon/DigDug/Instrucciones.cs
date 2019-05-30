class Instrucciones : Menu
{
    public Instrucciones()
    {
        bg = new Image("data/menu_partidas.png");
        font24 = new Font("data/Joystix.ttf", 20);
        continuar = true;
    }

    public void DetectarTeclas()
    {
        SdlHardware.ShowHiddenScreen();
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            SdlHardware.Pause(100);
            continuar = false;
        }
    }

    public override void DibujarInterfaz()
    {

        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(bg, 0, 0);
        SdlHardware.WriteHiddenText("Instrucciones: ",
            100, 50,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("-Pulsa las flechas de control",
            100, 150,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("para moverte / seleccionar opciones",
            100, 200,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("-Pulsa Espacio para hablar con ",
            100, 300,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("Personajes/Interactuar en general",
            100, 350,
            0xC0, 0xC0, 0xC0,
            font24);
        SdlHardware.WriteHiddenText("-Pulsa M para acceder al menú ",
            100, 450,
            0xC0, 0xC0, 0xC0,
            font24);

        SdlHardware.WriteHiddenText("Pulsa Espacio para cerrar esto",
                100, 600,
                0xC0, 0xC0, 0xC0,
                font24);

        SdlHardware.ShowHiddenScreen();

        SdlHardware.Pause(40);


    }
    public void Run()
    {
        do
        {
            DibujarInterfaz();
            DetectarTeclas();
        } while (continuar);
    }
}
