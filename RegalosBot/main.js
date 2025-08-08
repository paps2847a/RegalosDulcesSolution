const { Client, LocalAuth } = require('whatsapp-web.js');
const qrcode = require('qrcode-terminal');
const { SendPostPayloads, sleep } = require('./utilities/utilsProp.js'); // Assuming this is the correct path to your utility function

const packageJson = require('./package.json');
const apiUrl = packageJson.apiUrl;

const client = new Client({
    authStrategy: new LocalAuth()
});

client.on('ready', async () => {
    console.log('Client is ready!');
});

client.on('message', async (msg) => {

    if(msg.body === 'hola')
    {
        const opciones = [
            "1. /precios - Para Recibir la lista de precios de los biscochos por tamano.",
            "2. /orden - Para que describas un poco sobre como quieres tu torta.",
            "3. /lista - Para que puedas ver los precios de nuestros diferentes postres ya preparados.",
            "4. /contacto - Para contactar con un administrador."
        ];
        await msg.reply(`Bienvenido a regalos bot, soy el asistente automatico.Te presento las opciones que puedes usar:`);
        sleep(700);
        await msg.reply(opciones.join('\n'));
    }

    if(msg.body === '/precios' || msg.body === 'precios')
    {
        await msg.reply(`Espera un momento por favor, estoy buscando los precios...`);
        let tamanosTortas = await SendPostPayloads(apiUrl, 'tamano', 'getall', {});
        if(tamanosTortas.length === 0){
            await msg.reply(`Lo siento, no se encontraron tamanos de tortas.`);
            return;
        }
        let precios = tamanosTortas.map(tamano => `${tamano.desTam} - $${tamano.vendTam}`).join('\n');
        await msg.reply(`Estos son los precios de los biscochos por tamano:\n${precios}`);
        sleep(700);
        await msg.reply(`Si quieres hacer un pedido, por favor escribe /orden.`);
    }

    if(msg.body === '/orden' || msg.body === 'orden')
    {
        await msg.reply(`Por favor, describe como quieres tu torta y te ayudare a crearla.`);
    }

    if(msg.body === '/lista' || msg.body === 'lista')
    {
    }

    if(msg.body === '/contacto' || msg.body === 'contacto')
    {
        await msg.reply("Tu solicitud ha sido recibida, dentro de poco alguien se estara contactando contigo.");
    }

    console.log("Mensaje recibido: ", msg.body);
});

client.on('qr', qr => {
    qrcode.generate(qr, {small: true});
});

client.initialize();

const serv = Bun.serve({
    port: 4010,
    routes: {
        '/wsbot/getusergroups': async (req) => {
            let contacts = await client.getContacts();
            let groups = contacts.filter(contact => contact.id.user.includes("-"));

            if(groups.length === 0)
                return Response.json({ message: 'No se encontraron grupos.' });

             return Response.json(groups);
        },
        '/wsbot/sendwsmsg': async (req) => {
            POST: async (req) => {
                let { groupsIds, msg } = await req.json();
                let groupsArray = groupsIds.split('|');
                if(groupsArray.length === 0 || !msg)
                    return Response.json({ message: 'Faltan datos necesarios.' }, { status: 400 });

                for (let groupId of groupsArray) {
                    let chat = await client.getChatById(groupId);
                    chat.sendMessage(msg);

                    await Bun.sleep(1000); // Espera 1 segundo entre mensajes para evitar problemas de rate limiting
                }

                return Response.json({ message: 'Mensajes enviados correctamente.' });
            }
        }
    },
    fetch(req) {
        return new Response("Not Found", { status: 404 });
    }
});

console.log(`Servidor Bun corriendo en el puerto ${serv.port}`);