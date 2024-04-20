import { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';

function BitcoinEvents({ onNewEvent }) {
    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7057/bitcoinHub")
            .build();

        connection.start()
            .then(() => {
                console.log("SignalR connected");
            })
            .catch(err => console.error("SignalR connection bananas:", err));

        connection.on("ReceiveBitcoinEvent", (eventType, hash) => {
            console.log(`Received ${eventType} event: ${hash}`);
            //LD Notify parent component with data received
            if (onNewEvent) {
                onNewEvent(eventType, hash);
            }
        });

        //LD Stop the connection only when the component is unmounted
        return () => {
            console.log("BitcoinEvents component unmounted");

        };
    }, []); 

    return null;
}

export default BitcoinEvents;