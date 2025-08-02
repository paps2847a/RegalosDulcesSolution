// Este archivo contiene la función sleep y GetTamanos
const sleep = (ms) => new Promise(resolve => setTimeout(resolve, ms));

// Asegúrate de que `apiUrl` esté definido o se pase como parámetro si es una variable global o de entorno
// Para este ejemplo, asumiremos que apiUrl está definido en algún lugar accesible o que lo pasarás al importar.
// const apiUrl = "http://tu-api.com"; // Ejemplo: Descomenta y ajusta si apiUrl no es global
async function SendPostPayloads(apiUrl, entity, endpoint, data) {
    try {
        let settings = {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        };

        // Asumiendo que `apiUrl` está definido en el ámbito donde se ejecuta esta función,
        // o que lo manejarás externamente.
        let response = await fetch(`${apiUrl}/${entity}/${endpoint}`, settings); 
        
        if (response.ok === false) {
            // El error 'ex' en tu código original no está definido aquí,
            // probablemente querías usar un mensaje o el propio error de la respuesta.
            console.error("Error al obtener los tamaños (respuesta no exitosa)."); 
            return [];
        }
        let dataResp = await response.json();
        if(dataResp.error) {
            console.error("Error al obtener los tamaños:", dataResp.error);
            return [];
        }

        return dataResp.data || [];
    } catch (ex) {
        console.error("Error al obtener los tamaños:", ex);
        return [];
    }
}

// --- Exportación de las funciones ---
// Exportamos un objeto que contiene ambas funciones
module.exports = {
    sleep: sleep,
    SendPostPayloads: SendPostPayloads
};