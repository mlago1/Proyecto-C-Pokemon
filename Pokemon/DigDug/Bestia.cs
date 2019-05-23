using System;
using System.Collections.Generic;
using System.IO;

public class Bestia : Sprite
{
    protected string nombre;
    protected int nivel,vida;
    List<Ataque.ataque> ataques;
    Random r = new Random();
    public Bestia(string nombre, string nombreImagen)
        : base (nombreImagen)
    {
        this.nombre = nombre;
        nivel = new Random().Next(1,100);
        vida = nivel * 2;
        width = 256;
        height = 282;
        ataques = new List<Ataque.ataque>();
        CargarAtaques();
    }

    public string GetNombre()
    {
        return nombre;
    }

    public List<Ataque.ataque> GetAtaques()
    {
        return ataques;
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

    private void CargarAtaques()
    {
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_ataques.txt");
            for (int i = 0; i < 4; i++)
            {
                int indiceRandom = r.Next(0, leer.Length);
                string[] cortarLinea = leer[indiceRandom].Split(';');
                this.ataques.Add(new Ataque.ataque(cortarLinea[0],cortarLinea[1],Convert.ToInt32(cortarLinea[2])));
            }
        }catch(Exception e)
        {

        }
    }
}
