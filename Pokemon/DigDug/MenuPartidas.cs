class MenuPartidas
{

    Image bg;
    Font font24;

    public MenuPartidas()
    {
        bg = new Image("data/menu_partidas");
        font24 = new Font("data/Joystix.ttf", 24);
    }

    public void Run()
    {
        do
        {
            SdlHardware.ClearScreen();

            SdlHardware.DrawHiddenImage(bg, 0, 0);

            SdlHardware.ShowHiddenScreen();

            SdlHardware.Pause(1);
        } while (true);
    }

}

