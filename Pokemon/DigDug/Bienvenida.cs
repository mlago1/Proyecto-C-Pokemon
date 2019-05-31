class Bienvenida : Menu
{
    public Bienvenida()
    {
        bg = new Image("data/pantalla_principal.jpg");
        font24 = new Font("data/Joystix.ttf", 35);
    }

    public void Run()
    {
        Sound bgSound = new Sound("data/sonidos/pantalla_titulo.mp3");
        bgSound.BackgroundPlay();
        do
        {
            SdlHardware.ClearScreen();

            SdlHardware.DrawHiddenImage(bg, 0, 0);
            SdlHardware.WriteHiddenText("Pulsa Espacio para",
                100, 550,
                0xC0, 0xC0, 0xC0,
                font24);
            SdlHardware.WriteHiddenText("Continuar",
                200, 600,
                0xC0, 0xC0, 0xC0,
                font24);

            SdlHardware.ShowHiddenScreen();

            SdlHardware.Pause(1);

        } while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
        bgSound.StopMusic();
        SdlHardware.Pause(100);
    }
}


