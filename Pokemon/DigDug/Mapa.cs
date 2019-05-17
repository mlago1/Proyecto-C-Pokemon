using System;
using System.Collections.Generic;
using System.IO;

public class Mapa
{
     public List<Arbol> Arboles { get; set; }
     public List<Edificio> Edificios { get; set; }
     public List<Hierba> Hierbas { get; set; }

    public Mapa()
    {
        Arboles = new List<Arbol>();
        Edificios = new List<Edificio>();
        Hierbas = new List<Hierba>();
    }

    public void CargarMapa(string nombre)
    {
        List<string> lineas;
        int actualX, actualY;

        try
        {
            lineas = new List<string>(File.ReadAllLines(nombre));
        }
        catch (Exception e)
        {
            lineas = new List<string>();
        }
        Sprite aux = new Sprite();

        for (int i = 0; i < lineas.Count; i++)
        {
            String linea = lineas[i];
            actualY = i * aux.height;

            for (int j = 0; j < linea.Length; j++)
            {
                actualX = j * aux.width;
                
                switch (linea[j])
                {
                    case 'A':
                        Arbol a = new Arbol("data/tree.png");
                        a.MoveTo(actualX, actualY);
                        Arboles.Add(a);
                        break;
                       
                    case 'E':
                        Edificio e = new Edificio("data/casa_amarilla.png");
                        e.MoveTo(actualX , actualY );
                        Edificios.Add(e);
                        break;

                    case 'H':
                        Hierba h = new Hierba("data/hierba_alta.png");
                        h.MoveTo(actualX, actualY);
                        Hierbas.Add(h);
                        break;
                }

            }
        }
    }
}