using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegistroEstudiantes.Tests;

[TestClass]
public class Test1 : PageTest
{
    [TestMethod]
    public async Task VerificarIndexPrestamos()
    {
        // Abrir el Index de Préstamos
        await Page.GotoAsync("https://localhost:7038/Prestamo/Index");

        // Comprobar que aparece el título
        await Expect(Page.GetByText("Registro de Préstamos"))
            .ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task VerificarBotonCrear()
    {
        // Abrir el Index de Préstamos
        await Page.GotoAsync("https://localhost:7038/Prestamo/Index");

        // Presionar el botón Crear
        await Page.GetByRole(AriaRole.Link, new() { Name = "Crear" })
            .ClickAsync();

        // Comprobar que abrió la página Create
        await Expect(Page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex(@"/Prestamo/Create/?$")
        );
    }



    [TestMethod]
    public async Task VerificarCrearPrestamo()
    {
        // Abrir el formulario
        await Page.GotoAsync("https://localhost:7038/Prestamo/Create");

        // Seleccionar el primer estudiante disponible
        var estudiante = Page.Locator("select").Nth(0);

        await Expect(estudiante.Locator("option"))
            .Not.ToHaveCountAsync(1);

        var idEstudiante = await estudiante
            .Locator("option:not([value='0'])")
            .First.GetAttributeAsync("value");

        await estudiante.SelectOptionAsync(idEstudiante!);

        // Seleccionar el primer libro disponible
        var libro = Page.Locator("select").Nth(1);

        await Expect(libro.Locator("option"))
            .Not.ToHaveCountAsync(1);

        var idLibro = await libro
            .Locator("option:not([value='0'])")
            .First.GetAttributeAsync("value");

        await libro.SelectOptionAsync(idLibro!);

        // Guardar el préstamo
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar" })
            .ClickAsync();

        // Comprobar que regresó al Index
        await Expect(Page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex(@"/Prestamo/Index/?$")
        );
    }


    [TestMethod]
    public async Task VerificarModificarPrestamo()
    {
        // Abrir el préstamo 18
        await Page.GotoAsync("https://localhost:7038/Prestamo/Edit/18");

        // Comprobar que abrió Edit
        await Expect(Page.GetByText("Modificar Préstamo"))
            .ToBeVisibleAsync();

        // Cambiar la fecha del préstamo
        await Page.Locator("input[type='date']")
            .FillAsync("2026-10-11");

        // Guardar los cambios
        await Page.GetByRole(AriaRole.Button, new() { Name = "Modificar" })
            .ClickAsync();

        // Comprobar que regresó al Index
        await Expect(Page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex(@"/Prestamo/Index/?$")
        );

        // Verificar la fecha del préstamo 18 en la tabla
        var fila = Page.Locator("tr")
            .Filter(new() { Has = Page.Locator("td").GetByText("18", new() { Exact = true }) });

        await Expect(fila.GetByText("11/10/2026"))
            .ToBeVisibleAsync();
    }


    [TestMethod]
    public async Task VerificarEliminarPrestamo()
    {
        // ID de un préstamo creado exclusivamente para pruebas
        int prestamoId = 0; // Sustituir por el ID real

        await Page.GotoAsync(
            $"https://localhost:7038/Prestamo/Edit/{prestamoId}"
        );

        // Abrir el modal de eliminación
        await Page.GetByRole(AriaRole.Button, new() { Name = "Eliminar" })
            .ClickAsync();

        // Comprobar que aparece el modal
        var modal = Page.Locator("#eliminarModal");

        await Expect(modal).ToBeVisibleAsync();

        // Confirmar la eliminación
        await modal.GetByRole(AriaRole.Button, new() { Name = "Eliminar" })
            .ClickAsync();

        // Comprobar que regresó al Index
        await Expect(Page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex(@"/Prestamo/Index/?$")
        );

        // Comprobar que ya no aparece el préstamo
        var fila = Page.Locator("tbody tr").Filter(
            new()
            {
                Has = Page.Locator("td").GetByText(
                    prestamoId.ToString(),
                    new() { Exact = true }
                )
            }
        );

        await Expect(fila).ToHaveCountAsync(0);
    }


    [TestMethod]
    public async Task VerificarValidacionesPrestamo()
    {
        await Page.GotoAsync("https://localhost:7038/Prestamo/Create");

        // Guardar sin seleccionar estudiante ni libro
        await Page.GetByRole(AriaRole.Button, new() { Name = "Guardar" })
            .ClickAsync();

        // Comprobar que permanece en Create
        await Expect(Page).ToHaveURLAsync(
            new System.Text.RegularExpressions.Regex(@"/Prestamo/Create/?$")
        );

        // Comprobar que aparecen errores de validación
        await Expect(Page.Locator(".validation-message").First)
            .ToBeVisibleAsync();
    }

}
