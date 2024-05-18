import React, { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';

/**
 * "BitcoinEvents.js" is the component used to create the SignalR connection to the backend, a bitcoin hub. Front end 
 * listen for new transaction and block events. When an event is received, then calls the
 * "onNewEvent" callback(passed as a prop)
 * 
 * Props:
 * - onNewEvent: explained in the summary above
 */
function BitcoinEvents({ onNewEvent }) {
  useEffect(() => {
    //LD Creating a new SignalR connection then starting it
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7057/bitcoinHub")
      .build();

    connection.start()
      .then(() => {
        console.log("SignalR connected");
      })
      .catch(err => console.error("SignalR connection error:", err));

    //LD this is the handle for when transactions events are received
    const handleTransactionEvent = (eventType, hash) => {
      if (onNewEvent) {
        onNewEvent(eventType, hash);
      }
    };

    //LD similarly as per the transaction handle but for blocks
    const handleBlockEvent = (blockData) => {
      console.log(`Received Block Data ${blockData}`);
      if (onNewEvent) {
        onNewEvent(null, blockData);
      }
    };

    //LD this is the set up of SignalR event listeners
    connection.on("ReceiveTransactionEvent", handleTransactionEvent);
    connection.on("ReceiveBlockEvent", handleBlockEvent);

    //LD connection clean up(SignalR connection when the component unmounts)
    return () => {
      console.log("BitcoinEvents component unmounted");
      connection.stop();
      connection.off("ReceiveTransactionEvent", handleTransactionEvent);
      connection.off("ReceiveBlockEvent", handleBlockEvent);
    };
  }, []); //LD effect should run once. Looking at browser console stream seems working fine.

  return null;
}

export default BitcoinEvents;

