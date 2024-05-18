# CA3 Luca D'Angelo

1. **High Level Architecture**
    - Interaction Flow Diagram: ![Flow](ReadmeImages/img_flow.png)
    - Overview:
        - Back End
            - The back end was implemented utilizing ASP.NET Core API with SignalR for real-time communication and NBitcoin for interacting with the Bitcoin network. 
                1. SignalR (used for real-time web functionality, allowing server-side code to push content to connected clients instantly) is used by “BitcoinHub” for broadcasting Bitcoin events (transactions, blocks) to all connected clients. 
                2. NBitcoin (.NET framework library) was used for handling Bitcoin protocol interactions and blockchain operations. So connect to Bitcoin nodes, process blockchain data and handle Bitcoin protocols.
                3. Code is provided with summaries and descriptions.
            - Note: an option was to use a .NET Core console app, but I wanted to provide functionality to easily send cancellation tokens to the controller action when done with bitcoin observation.
            - References: 
                - https://dotnet.microsoft.com/en-us/apps/aspnet/signalr
                - https://metacosa.github.io/NBitcoin/api/ 
        - Front End
            - The front end was implemented using React. Main responsibility is to display the latest Bitcoin transactions and more importantly blocks of transactions in a user-friendly interface. Key parts are:
                1. State Management: this wraps managing of state for latest Bitcoin events, block queue, active tab, total pages for transactions, pagination status and expanded transactions.
                2. Event Handling: listens (in infinite loop until cancellation token) for new Bitcoin events (transactions and blocks) from the backend and updates the state accordingly.
                3. Tabs: Uses react-bootstrap Tabs to switch between "Blocks" and "Transactions" views. 
                4. Components: Renders BitcoinEvents, BlockDisplay, and TransactionDisplay components.
                5. Interaction with back end: “BitcoinEvents.js” component connects to the backend's SignalR hub at https://localhost:7057/bitcoinHub (static) to receive real-time updates on Bitcoin transactions and blocks. When events are received the latest block or transaction state gets updated. Then data parsing/massaging and UI rendering! Yes super mega cool.

2. **Code**
    - Summaries in code:
        1. Back End
            - NodeInfoController.cs
            - CreateBlockData.cs
            - ProcessRawTransactionData.cs
        2. Front End
            - App.js
            - CustomPagination.js
            - TransactionDisplay.js
            - BlockDisplay.js
            - BitcoinEvents.js

3. How to build and run the application. Any possible dependency

4. How to use it. Screen shots and use cases

