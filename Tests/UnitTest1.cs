namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void SuscribirUsuario_AgregaUsuarioALaLista()
        {
            Tema tema = new Tema(1, "Tema Deportivo");
            Usuario usuario = new Usuario(1, "Ivan");

            tema.suscribirUsuario(usuario);

            Assert.Contains(usuario, tema.UsuariosSuscritos);
        }

        [Fact]
        public void AlertarTodos_AlertaATodosLosUsuariosSuscritos()
        {
            Tema tema = new Tema(1, "Musica");
            Usuario usuario1 = new Usuario(1, "Juan");
            Usuario usuario2 = new Usuario(2, "Pedro");
            tema.suscribirUsuario(usuario1);
            tema.suscribirUsuario(usuario2);

            tema.alertarTodos(DateTime.Now, TipoAlerta.URGENTE);

            Assert.True(usuario1.Alertas.Count > 0);
            Assert.True(usuario2.Alertas.Count > 0);
        }

        [Fact]
        public void AlertarUsuario_AlertaUsuarioEspecifico()
        {
            Tema tema = new Tema(1, "Tema de Prueba");
            Usuario usuario = new Usuario(1, "Usuario de Prueba");
            tema.suscribirUsuario(usuario);

            tema.alertarUsuario(usuario, DateTime.Now, TipoAlerta.INFORMATIVA);

            Assert.True(usuario.Alertas.Count > 0);
        }

        [Fact]
        public void RecibirAlerta_AgregaAlertaALaLista()
        {
            Usuario usuario = new Usuario(1, "Pepe");
            Alerta alerta = new Alerta(1, "FutbolArgentino", DateTime.Now, TipoAlerta.URGENTE, true);

            usuario.recibirAlerta(alerta);

            Assert.Contains(alerta, usuario.Alertas);
        }

        [Fact]
        public void ObtenerAlertasNoLeidasNoExpiradas_ObtieneAlertasCorrectas()
        {
            Usuario usuario = new Usuario(1, "Usuario");
            DateTime fechaActual = DateTime.Now;
            Alerta alerta1 = new Alerta(1, "Tema Urgente", fechaActual.AddHours(1), TipoAlerta.URGENTE, true);
            Alerta alerta2 = new Alerta(2, "Tema Informativo", fechaActual.AddHours(2), TipoAlerta.INFORMATIVA, false);
            Alerta alerta3 = new Alerta(3, "Tema Informativo Vencido", fechaActual.AddHours(-1), TipoAlerta.INFORMATIVA, true);

            usuario.recibirAlerta(alerta1);
            usuario.recibirAlerta(alerta2);
            usuario.recibirAlerta(alerta3);

            var alertasNoLeidasNoExpiradas = usuario.obtenerAlertasNoLeidasNoExpiradas();

            Assert.Equal(2, alertasNoLeidasNoExpiradas.Count);
            Assert.Contains(alerta2, alertasNoLeidasNoExpiradas);
            Assert.DoesNotContain(alerta3, alertasNoLeidasNoExpiradas);
        }

        [Fact]
        public void ObtenerAlertasNoExpiradasPorTema_ObtieneAlertasCorrectas()
        {
            Usuario usuario = new Usuario(1, "Usuario");
            Tema tema = new Tema(1, "Tema 1");
            Tema tema2 = new Tema(3, "Tema 2");
            DateTime fechaActual = DateTime.Now;
            Alerta alerta1 = new Alerta(1, "Tema 1", fechaActual.AddHours(1), TipoAlerta.URGENTE, true);
            Alerta alerta2 = new Alerta(2, "Tema 1", fechaActual.AddHours(2), TipoAlerta.INFORMATIVA, false);
            Alerta alerta3 = new Alerta(3, "Tema 2", fechaActual.AddHours(-1), TipoAlerta.INFORMATIVA, true);
            Alerta alerta4 = new Alerta(4, "Tema 2", fechaActual.AddHours(3), TipoAlerta.INFORMATIVA, true);

            usuario.recibirAlerta(alerta1);
            usuario.recibirAlerta(alerta2);
            usuario.recibirAlerta(alerta3);
            usuario.recibirAlerta(alerta4);

            var alertasNoExpiradasPorTema = usuario.obtenerAlertasNoExpiradasPorTema(tema);
            var alertasNoExpiradasPorOtroTema = usuario.obtenerAlertasNoExpiradasPorTema(tema2);

            Assert.Equal(2, alertasNoExpiradasPorTema.Count);
            Assert.Single(alertasNoExpiradasPorOtroTema);
            Assert.Equal(alerta2, alertasNoExpiradasPorTema[1].alerta);
            Assert.False(alertasNoExpiradasPorTema[1].fueParaTodos);
        }
    }
}