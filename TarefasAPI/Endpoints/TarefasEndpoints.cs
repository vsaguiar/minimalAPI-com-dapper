using Dapper.Contrib.Extensions;
using TarefasAPI.Data;
using static TarefasAPI.Data.TarefaContext;

namespace TarefasAPI.Endpoints;

public static class TarefasEndpoints
{

    public static void MapTarefasEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => $"Bem-vindo a API Tarefas {DateTime.Now}");

        // Endpoint para retornar uma lista de tarefas
        app.MapGet("/tarefas", async (GetConnection connectionGetter) =>
        {
            using var con = await connectionGetter();
            var tarefas = con.GetAll<Tarefa>().ToList();

            if (tarefas is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(tarefas);
        });


        // Endpoint para retornar uma única tarefa
        app.MapGet("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();
            var tarefa = con.Get<Tarefa>(id);

            if (tarefa is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(tarefa);
        });


        // Endpoint para incluir uma nova tarefa
        app.MapPost("/tarefas", async(GetConnection connectionGetter, Tarefa Tarefa) => 
        {
            using var con = await connectionGetter();
            var id = con.Insert(Tarefa);
            return Results.Created($"/tarefas/{id}", Tarefa);
        });


        // Endpoint para alterar uma tarefa
        app.MapPut("/tarefas", async (GetConnection connectionGetter, Tarefa Tarefa) =>
        {
            using var con = await connectionGetter();
            var id = con.Update(Tarefa);
            return Results.Ok();
        });


        // Endpoint para excluir uma tarefa
        app.MapDelete("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
        {
            using var con = await connectionGetter();

            var deleted = con.Get<Tarefa>(id);
            if (deleted is null)
            {
                return Results.NotFound();
            }
            con.Delete(deleted);
            return Results.Ok(deleted);
        });

    }
}
