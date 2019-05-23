using System;

public class Ataque
{
    public struct ataque
    {
        public string nombre;
        public string tipo;
        public int poder;

        public ataque(string nombre, string tipo, int poder)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.poder = poder;
        }
    }
}
