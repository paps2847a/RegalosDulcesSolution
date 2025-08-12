'use strict';

const { Client, LocalAuth } = require('whatsapp-web.js');
const qrcode = require('qrcode-terminal');
const cheerio = require('cheerio');
const { SendPostPayloads } = require('./utilities/utilsProp.js');

const packageJson = require('./package.json');
const apiUrl = packageJson.apiUrl;
const PORT = 4010;

let fetchedData = null;

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
        await Bun.sleep(1000);
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
        await Bun.sleep(1000);
        await msg.reply(`Si quieres hacer un pedido, por favor escribe /orden.`);
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
    port: PORT,
    routes: {
        '/dolar/now':{
            GET: async (req) => {
                if(fetchedData == null || (Date.now() - fetchedData.timestamp >= 3600000)){
                    let response = await fetch('https://www.bcv.org.ve/', {
                        tls: {
                            rejectUnauthorized: false // Permite conexiones a sitios con certificados no verificados
                        }
                    });
                    if(!response.ok)
                        return Response.json({ message: 'Error al obtener los datos del BCV' }, { status: 500 });

                    let html = await response.text();
                    if(html.length === 0)
                        return Response.json({ message: 'Error al obtener los datos del BCV' }, { status: 500 });

                    const $ = cheerio.load(html);
                    const dolarValor = parseFloat(
                        $("#dolar > div:nth-child(1) > div:nth-child(1) > div:nth-child(2) > strong:nth-child(1)")
                        .text()
                        .trimStart()
                        .trimEnd()
                        .replace(",", ".")
                    ).toFixed(2);
                                    
                    const fecha = $(".pull-right > span:nth-child(1)")
                                .text()
                                .trimStart()
                                .trimEnd();

                    fetchedData = {
                        valor: dolarValor,
                        fecha: fecha,
                        timestamp: Date.now()
                    };

                    return Response.json({
                        valor: dolarValor,
                        fecha: fecha
                    }, { status: 200 });
                }

                return Response.json({
                    valor: fetchedData.valor,
                    fecha: fetchedData.fecha
                }, { status: 200 });
            }
        },
        '/wsbot/getusergroups': {
                GET: async (req) => {
                    let contacts = await client.getContacts();
                    let groups = contacts.filter(contact => contact.id.user.includes("-"));

                    if(groups.length === 0)
                        return Response.json({ message: 'No se encontraron grupos.' });

                    return Response.json(groups);
            }
        },
        '/wsbot/sendwsmsg': {
            POST: async (req) => {
                try{
                    let { groupsIds, msg } = await req.json();
                
                let groupsArray = groupsIds.split('|');
                if(groupsArray.length === 0 || !msg)
                    return Response.json({ message: 'Faltan datos necesarios.' }, { status: 400 });

                for (let groupId of groupsArray) {
                    await client.sendMessage(groupId, msg)
                    await Bun.sleep(1000); // Espera 1 segundo entre mensajes para evitar problemas de rate limiting
                }

                return Response.json({ message: 'Mensajes enviados correctamente.' });
                }
                catch (error) {
                    console.error("Error al enviar el mensaje:", error);
                    return Response.json({ message: 'Error al enviar el mensaje.' }, { status: 500 });
                }
            }
        }
    },
    fetch(req) {
        return new Response("Not Found", { status: 404 });
    }
});

console.log(`Servidor Bun corriendo en el puerto ${serv.port}`);