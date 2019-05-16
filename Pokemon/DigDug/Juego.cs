using System;
class Juego
{
    protected Jugador protagonista;
    protected bool bucle;
    protected Image fondo;
    protected Mapa mapa;

    public Juego(Jugador protagonista)
    {
        bucle = true;
        this.protagonista = protagonista;
        this.protagonista.MoveTo(512, 450);
        fondo = new Image("data/fondo_juego.jpg");
        mapa = new Mapa();
        mapa.CargarMapa("data/mapa.txt");
    }

    public void DibujarJuego()
    {
        SdlHardware.ClearScreen();
        SdlHardware.DrawHiddenImage(fondo, 0, 0);
        foreach (Sprite arbol in mapa.Arboles)
        {
            arbol.DrawOnHiddenScreen();
        }
        foreach (Sprite edificio in mapa.Edificios)
        {
            edificio.DrawOnHiddenScreen();
        }
        protagonista.DrawOnHiddenScreen();
        SdlHardware.ShowHiddenScreen();
    }

    public void MoverMundo(int X, int Y)
    {
        foreach (Sprite arbol in mapa.Arboles)
        {
            arbol.MoveTo(arbol.GetX()+X,arbol.GetY()+Y);
        }
        foreach (Sprite edificio in mapa.Edificios)
        {
            edificio.MoveTo(edificio.GetX() + X, edificio.GetY() + Y);
        }
    }

    public void ComprobarTeclas()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_DOWN))
        {
            if(protagonista.PuedeMoverse(protagonista,mapa.Arboles,mapa.Edificios))
                MoverMundo(0,-5);

            protagonista.ChangeDirection(Sprite.DOWN);
            protagonista.NextFrame();
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_UP))
        {
            if (protagonista.PuedeMoverse(protagonista, mapa.Arboles, mapa.Edificios))
                MoverMundo(0,5);

            protagonista.ChangeDirection(Sprite.UP);
            protagonista.NextFrame();
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_LEFT))
        {
            if (protagonista.PuedeMoverse(protagonista, mapa.Arboles, mapa.Edificios))
                MoverMundo(5,0);

            protagonista.ChangeDirection(Sprite.LEFT);
            protagonista.NextFrame();
        }
        else if (SdlHardware.KeyPressed(SdlHardware.KEY_RIGHT))
        {
            if (protagonista.PuedeMoverse(protagonista, mapa.Arboles, mapa.Edificios))
                MoverMundo(-5, 0);

            protagonista.ChangeDirection(Sprite.RIGHT);
            protagonista.NextFrame();
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_K))
        {
            SdlHardware.Pause(100);
            Pokemon.Run();
        }
    }

    public void Run()
    {
        do
        {
            DibujarJuego();
            ComprobarTeclas();
            SdlHardware.Pause(1);
        } while (bucle);
    }
}

