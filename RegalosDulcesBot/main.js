const { Client, LocalAuth } = require('whatsapp-web.js');
const qrcode = require('qrcode-terminal');
const { SendPostPayloads, sleep } = require('./utilities/utilsProp.js'); // Assuming this is the correct path to your utility function
const express = require('express');
const app = express();

const packageJson = require('./package-lock.json');
const apiUrl = packageJson.apiUrl || 'http://localhost:3000'; // Default to localhost if not specified

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

//SERVIDOR EXPRESS

//MIDDLEWARE DE JSON
app.use(express.json());

app.post('/wsbot/sendwsmsg', async (req, res) => {
    let { groupsIds, msg } = req.body.parameters;
    let groupsArray = groupsIds.split('|');
    if(groupsArray.length === 0 || !msg)
        return res.status(400).json({ message: 'Faltan datos necesarios.' });

    for (let groupId of groupsArray) {
        let chat = await client.getChatById(groupId);
        chat.sendMessage(msg);

        await sleep(1000); // Espera 1 segundo entre mensajes para evitar problemas de rate limiting
    }

    return res.status(200).json({ message: 'Mensajes enviados correctamente.' });
});

app.get('/wsbot/getusergroups', async (req, res) => {
    let contacts = await client.getContacts();
    let groups = contacts.filter(contact => contact.id.user.includes("-"));
    if(groups.length === 0)
        return res.status(404).json({ message: 'No se encontraron grupos.' });

    return res.status(200).json(groups);
});

app.listen(4010, () => {
    console.log('El servidor esta corriendo en el puerto 4010');
});
