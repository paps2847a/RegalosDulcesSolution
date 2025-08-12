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
    SendPostPayloads: SendPostPayloads
};