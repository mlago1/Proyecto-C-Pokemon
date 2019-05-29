using System;
using System.Collections.Generic;
using System.IO;

public class Mapa
{
     public List<Arbol> Arboles { get; set; }
     public List<Edificio> Edificios { get; set; }
     public List<Hierba> Hierbas { get; set; }
     public List<Npc> Npcs { get; set; }
     public List<Pc> Pcs { get; set; }
     Random r;

    public Mapa()
    {
        Arboles = new List<Arbol>();
        Edificios = new List<Edificio>();
        Hierbas = new List<Hierba>();
        Npcs = new List<Npc>();
        Pcs = new List<Pc>();
        r = new Random();
    }

    private Npc CargarNpc()
    {
        Npc devolver = null;
        int numFrasesAnyadir = 3;
        try
        {
            string[] leer = File.ReadAllLines("data/npcs/lista_npc.txt");
            string[] leer2 = File.ReadAllLines("data/npcs/lista_frases.txt");

            int indice = r.Next(0, leer.Length);
            devolver = new Npc(new string[4][] {
                new string[] {leer[indice].Split(';')[0] },
                new string[] {leer[indice].Split(';')[1] },
                new string[] {leer[indice].Split(';')[2] },
                new string[] {leer[indice].Split(';')[3] } });

            for(int i = 0; i < numFrasesAnyadir; i++)
            {
                indice = r.Next(0, leer2.Length);
                if (!devolver.Dialogo.Contains(leer2[indice]))
                    devolver.Dialogo.Add(leer2[indice]);
            }
            
        }
        catch (Exception e)
        {

        }
        return devolver;
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
            string linea = lineas[i];
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

                    case 'N':
                        Npc n = CargarNpc();
                        n.MoveTo(actualX, actualY);
                        Npcs.Add(n);
                        break;

                    case 'P':
                        Pc p = new Pc("data/pc.png");
                        p.MoveTo(actualX, actualY);
                        Pcs.Add(p);
                        break;
                }

            }
        }
    }
}