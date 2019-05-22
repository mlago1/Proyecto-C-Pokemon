using System;

public class Bestia : Sprite
{
    protected string nombre;
    protected int nivel,vida;
    //List<Ataque> ataques; TO DO
    public Bestia(string nombre, string nombreImagen)
        : base (nombreImagen)
    {
        this.nombre = nombre;
        nivel = new Random().Next(1,100);
        vida = nivel * 2;
        width = 256;
        height = 282;
    }

    public string GetNombre()
    {
        return nombre;
    }

    public void SetNombre(string nombre)
    {
        this.nombre = nombre;
    }

    public int GetNivel()
    {
        return nivel;
    }

    public void SetNivel(int nivel)
    {
        this.nivel = nivel;
    }

    public int GetVida()
    {
        return vida;
    }

    public void SetVida(int vida)
    {
        this.vida = vida;
    }
}
