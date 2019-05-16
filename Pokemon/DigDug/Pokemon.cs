class Pokemon
{
    public static void Main()
    {
        Run();
    }

    public static void Run()
    {
        bool fullScreen = false;
        SdlHardware.Init(1024, 768, 24, fullScreen);
        Bienvenida b = new Bienvenida();
        b.Run();
        MenuPartidas m = new MenuPartidas();
        m.Run();
    }
}


