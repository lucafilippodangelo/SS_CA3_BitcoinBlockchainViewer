import React, { useEffect } from 'react';
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
            .catch(err => console.error("SignalR connection error:", err));

    
        const handleTransactionEvent = (eventType, hash) => {
            if (onNewEvent) {
                onNewEvent(eventType, hash);
            }
        };

        const handleBlockEvent = (blockData) => {
            console.log(`Received Block Data ${blockData}`);
            if (onNewEvent) {
                onNewEvent(null, blockData);
            }
        };

        connection.on("ReceiveTransactionEvent", handleTransactionEvent);
        connection.on("ReceiveBlockEvent", handleBlockEvent);

     
        return () => {
            console.log("BitcoinEvents component unmounted");
            connection.stop();
            connection.off("ReceiveTransactionEvent", handleTransactionEvent);
            connection.off("ReceiveBlockEvent", handleBlockEvent);
        };
    }, []); 

    return null;
}

export default BitcoinEvents;
