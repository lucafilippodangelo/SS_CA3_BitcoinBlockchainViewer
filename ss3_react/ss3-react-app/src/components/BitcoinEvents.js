import React, { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';

function BitcoinEvents({ onNewEvent }) {
    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7057/bitcoinHub")
            .build();

        // Start the connection when the component mounts
        connection.start()
            .then(() => {
                console.log("SignalR connected");
            })
            .catch(err => console.error("SignalR connection error:", err));

        // Define event handlers
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

        // Register event handlers
        connection.on("ReceiveTransactionEvent", handleTransactionEvent);
        connection.on("ReceiveBlockEvent", handleBlockEvent);

        // Clean up: stop the connection and remove event handlers when the component unmounts
        return () => {
            console.log("BitcoinEvents component unmounted");
            connection.stop();
            connection.off("ReceiveTransactionEvent", handleTransactionEvent);
            connection.off("ReceiveBlockEvent", handleBlockEvent);
        };
    }, []); // Empty dependency array ensures this effect runs only once

    return null;
}

export default BitcoinEvents;
