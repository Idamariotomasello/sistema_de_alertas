
using System.Text.RegularExpressions;

class challenge
{
    static void Main(string[] args) { }
}


public class Tema
{
    public int Id;
    public string NombreTema;
    public List<Usuario> UsuariosSuscritos;
    public Tema(int id, string nombre)
    {
        Id = id;
        NombreTema = nombre;
        UsuariosSuscritos = new List<Usuario>();
    }

    public void suscribirUsuario(Usuario usuario)
    {
        UsuariosSuscritos.Add(usuario);
    }

    public void alertarTodos(DateTime date, TipoAlerta tipo) // alerta a todos los usuarios suscriptos a el Tema en cuestion
    {
        var alerta = new Alerta(Id, NombreTema, date, tipo, true);
        UsuariosSuscritos.ForEach(usuario => usuario.recibirAlerta(alerta));
    }

    public void alertarUsuario(Usuario usuario, DateTime date, TipoAlerta tipo) // alerta a un usuario en especifico
    {
        var alerta = new Alerta(Id, NombreTema, date, tipo, false);
        usuario.recibirAlerta(alerta);
    }
}

public enum TipoAlerta
{
    URGENTE,
    INFORMATIVA
}


public class Usuario
{
    public int Id;
    public string Nombre;
    public List<Alerta> Alertas;// I1,I2,U1,I3,U2,I4 -> U1,U2,I1,I2,I3,I4 ->
    public Usuario(int id, string nombre)
    {
        Id = id;
        Nombre = nombre;
        Alertas = new List<Alerta>();
    }

    public void recibirAlerta(Alerta alerta)
    {
        Alertas.Add(alerta);
    }

    private List<Alerta> ordenarLista(List<Alerta> alertas)
    {
        var alertasUrgentes = alertas.Where(alerta => alerta.Tipo == TipoAlerta.URGENTE).Reverse();
        var alertasInformativas = alertas.Where(alerta => alerta.Tipo == TipoAlerta.INFORMATIVA);
        return alertasUrgentes.Concat(alertasInformativas).ToList();
    }
    public List<Alerta> obtenerAlertasNoLeidasNoExpiradas()
    {
        var alertas = Alertas.Where(alerta => !alerta.Leida && alerta.FechaExpiracion >= DateTime.Now).ToList();
        return ordenarLista(alertas);
    }

    public List<(Alerta alerta, bool fueParaTodos)> obtenerAlertasNoExpiradasPorTema(Tema tema)
    {
        var alertas = Alertas.Where(alerta => alerta.FechaExpiracion >= DateTime.Now && alerta.Tema == tema.NombreTema).ToList();
        return ordenarLista(alertas).Select(alerta => (alerta, alerta.FueParaTodos)).ToList(); // retorna una tupla indicando (alerta,fueParaTodos)
    }
}


public class Alerta
{
    public int Id;
    public string Tema;
    public DateTime FechaExpiracion;
    public TipoAlerta Tipo;
    public bool Leida = false;
    public bool FueParaTodos; // si es false significa que fue a un Usuario especifico la Alerta


    public Alerta(int id, string tema, DateTime fechaExpiracion, TipoAlerta tipo, bool fueParaTodos)
    {
        Id = id;
        Tema = tema;
        FechaExpiracion = fechaExpiracion;
        Tipo = tipo;
        FueParaTodos = fueParaTodos;
    }

    public bool AlertaLeida()
    {
        return Leida = true;
    }
}

//Ejercicio SQL
//Escribir una consulta SQL que traiga todos los clientes que han comprado en total más de 100,000$ 
//en los últimos 12 meses usando las siguientes tablas: 

//Clientes: ID, Nombre, Apellido

//Ventas: Fecha, Sucursal, Numero_factura, Importe, Id_cliente

//SELECT C.Nombre, C.Apellido, SUM(V.importe) AS Total FROM Ventas V 
//JOIN Clientes C on C.ID = V.Id_cliente
//WHERE V.Fecha >= DATEADD(MONTH, -12, GETDATE())
//GROUP BY V.Id_cliente, C.Nombre, C.Apellido
//HAVING SUM(V.importe) > 100000
//ORDER BY C.nombre, C.apellido