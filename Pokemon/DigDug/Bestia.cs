using System;
using System.Collections.Generic;
using System.IO;

public class Bestia : Sprite
{
    protected string nombre;
    protected int nivel,vida,maxVida;
    List<Ataque.ataque> ataques;
    Random r = new Random();
    public Bestia(string nombre, string nombreImagen)
        : base (nombreImagen)
    {
        this.nombre = nombre;
        nivel = new Random().Next(1,100);
        vida = nivel * 2;
        maxVida = vida;
        width = 256;
        height = 282;
        ataques = new List<Ataque.ataque>();
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

    public int GetMaxVida()
    {
        return maxVida;
    }

    public void SetMaxVida(int maxVida)
    {
        this.maxVida = maxVida;
    }

    public void CargarAtaques()
    {
        try
        {
            string[] leer = File.ReadAllLines("data/pokemons/lista_ataques.txt");
            for (int i = 0; i < 4; i++)
            {
                int indiceRandom = r.Next(0, leer.Length);
                string[] cortarLinea = leer[indiceRandom].Split(';');
                bool control = true;
                foreach (Ataque.ataque a in this.ataques)
                {
                    if(a.nombre == cortarLinea[0])
                    {
                        control = false;
                    }
                }
                if(control)
                    this.ataques.Add(new Ataque.ataque(cortarLinea[0],cortarLinea[1],Convert.ToInt32(cortarLinea[2])));
            }
        }catch(Exception e)
        {

        }
    }
}
