document.addEventListener("DOMContentLoaded", function () {
    iniciarSignalR();
});

function iniciarSignalR() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7243/apartamentosHub", {
            withCredentials: true // Importante para que funcione con CORS
        })
        .withAutomaticReconnect()
        .build();

    connection.start()
        .then(() => console.log("Conectado a SignalR"))
        .catch(err => console.error("Error al conectar con SignalR:", err));

    connection.on("RecargarDatos", function () {
        console.log("Se ha recibido una notificación para recargar los datos.");
        cargarApartamentos();
    });
}


async function cargarApartamentos() {
    try {
        const response = await fetch('/Apartamentos/IndexAll'); // Llama al controlador de ASP.NET MVC
        const html = await response.text();

        // Reemplazar solo la tabla de apartamentos en la vista
        document.querySelector("tbody").innerHTML =
            new DOMParser().parseFromString(html, "text/html").querySelector("tbody").innerHTML;
    } catch (error) {
        console.error("Error cargando apartamentos:", error);
    }
}



async function eliminarApartamento(id) {
    console.log("ID recibido:", id); 
    if (!id) {
        alert("Error: ID inválido");
        return;
    }

    try {
        let response = await fetch(`https://localhost:7243/api/Apartamento/${id}`, { method: "DELETE" });

        if (response.ok) {
            document.getElementById(`row-${id}`).remove();
        } else {
            alert("No se pudo eliminar el apartamento");
        }
    } catch (error) {
        console.error("Error en la solicitud", error);
    }
}
window.addEventListener("pageshow", function (event) {
    if (event.persisted) { // Detecta si la página se está cargando desde la caché
        location.reload(); // Recarga la página para obtener los datos actualizados
    }
});


document.getElementById("form-apartamento")?.addEventListener("submit", async function (e) {
    e.preventDefault();


    let data = {
        numero: document.getElementById("numero").value,
        usuarioResponsable: document.getElementById("usuarioResponsable").value,
        estado: document.getElementById("estado").value,
        torre: document.getElementById("torre").value,
        piso: parseInt(document.getElementById("piso").value),
        areaM2: parseFloat(document.getElementById("areaM2").value),
        descripcion: document.getElementById("descripcion").value
    };

    try {
        let response = await fetch("https://localhost:7243/api/Apartamento", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            alert("Apartamento creado correctamente");
            window.location.href = "/Apartamentos/IndexAll";
        } else {
            alert("Error al crear el apartamento");
        }

    } catch (error) {
        console.error("Error", error);
    }



})


document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("editForm");
    if (!form) return; // Evita errores si el formulario no existe en la página

    form.addEventListener("submit", async function (event) {
        event.preventDefault();
        const id = document.getElementById("apartamentoId").value;

        let data = {
            apartamentoId: parseInt(id),
            numero: document.getElementById("numero").value,
            usuarioResponsable: document.getElementById("usuarioResponsable").value,
            estado: document.getElementById("estado").value,
            torre: document.getElementById("torre").value,
            descripcion: document.getElementById("descripcion").value,
            piso: parseInt(document.getElementById("piso").value),
            areaM2: parseFloat(document.getElementById("areaM2").value)
        };

        try {
            const response = await fetch(`https://localhost:7243/api/Apartamento/${id}`, {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data)
            });

            if (response.ok) {
                alert("Apartamento actualizado correctamente");
                window.location.href = "/Apartamentos/IndexAll";
            } else {
                const errorText = await response.text();
                alert(`Error al actualizar: ${errorText}`);
            }
        } catch (error) {
            console.error("Error en la solicitud:", error);
            alert("Error al conectar con el servidor");
        }
    });
});
