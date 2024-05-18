# CA3 Secure Systems Development - D23125106 Luca D'Angelo

---

1. **Architecture**
2. **Code**
3. **Build and run the Application(including referenced dependencies)**
4. **Use Cases**

---

1. **High Level Architecture**
    - Interaction Flow Diagram: 
    
    ![Flow](ReadmeImages/img_flow.png)

    - Overview:
        - Back End
            - The back end is implemented utilizing ASP.NET Core API with SignalR for real-time communication and NBitcoin for interacting with the Bitcoin network. 
                1. SignalR (used for real-time web functionality, allowing server-side code to push content to connected clients instantly) is used by “BitcoinHub” for broadcasting Bitcoin events (transactions, blocks) to all connected clients. 
                2. NBitcoin (.NET framework library) is used for handling Bitcoin protocol interactions and blockchain operations. So connect to Bitcoin nodes, process blockchain data and handle Bitcoin protocols.
            - Note: an option was to use a .NET Core console app, but I wanted to provide functionality to easily send cancellation tokens to the controller action when done with bitcoin observation.
            - References: 
                - https://dotnet.microsoft.com/en-us/apps/aspnet/signalr
                - https://metacosa.github.io/NBitcoin/api/ 
        - Front End
            - The front end is implemented in React. Main responsibility is to display the latest Bitcoin transactions and more importantly blocks of transactions in a user-friendly interface. Key parts are:
                1. State Management: this wraps managing of state for latest Bitcoin events, block queue, active tab, total pages for transactions, pagination status and expanded transactions.
                2. Event Handling: listens (in infinite loop until cancellation token) for new Bitcoin events (transactions and blocks) from the backend and updates the state accordingly.
                3. Tabs: Uses react-bootstrap Tabs to switch between "Blocks" and "Transactions" views. 
                4. Components: Renders BitcoinEvents, BlockDisplay, and TransactionDisplay components.
                5. Interaction with back end: “BitcoinEvents.js” component connects to the backend's SignalR hub at https://localhost:7057/bitcoinHub (static in CA3 delivery) to receive real-time updates on Bitcoin transactions and blocks. When events are received the latest block or transaction state gets updated. Then data is parsed and minimally massaged prior UI rendering! 

2. **Code**
    - Summaries and logic description in code(key classes):
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

3. **Build and run the Application(including referenced dependencies)**

    - pull BE and FE from https://github.com/lucafilippodangelo/SS_CA3_BitcoinBlockchainViewer.git (Eoin you are collaborator)
    - run backend from terminal 
      1. sit in your local folder where .net core solution file is located(example "C:\Users\Luca\TUD\Web_Application_Architectures_10\SS_CA3_BitcoinBlockchainViewer\ss3_back>"
      2. then execute "dotnet build" 
      3. then execute "dotnet run"

      ![Flow](ReadmeImages/BE_001.png)

      4. open in browswer "https://localhost:7057/swagger/index.html" 
      5. in "NodeInfo" controller action click "get" 
      6. then click "try it out" 
      7. then click "Execute"

      ![Flow](ReadmeImages/FE_002.png)
      
      8. transactions start to be received from node and logged at console

      ![Flow](ReadmeImages/CO_003.png)
      
      NOTE: depending on how busy the node is, transactiond may not be received. In that case please update IP in Back End
      ![Flow](ReadmeImages/BE_009.png)

    - run front end from terminal
      1. sit in your local folder where the root of the react solution is. Example    "C:\Users\Luca\TUD\Web_Application_Architectures_10\SS_CA3_BitcoinBlockchainViewer\ss3_react\ss3-react-app"
      2. execute "npm install"
      3. execute "npm start"

    ![Flow](ReadmeImages/UI_004.png)

      - NOTE: if your front end does not run on port 3000 you need to update cors setup in backend, then rebuild. 

        ![Flow](ReadmeImages/BE_004.png)

4. **Use Cases**

    After executing step 3(building and running), open "http://localhost:3000" will be possible to see transactions starting to be rendered in transaction tab. The very last transaction received from Back end will be displayed in first row of the table. Open image in a new tab to see it fullsize.
    
    ![Flow](ReadmeImages/UI_005.png)

    When a block is received it will be diplayed in "Blocks" tab of the web application.
    - In yellow, mapping of the block
    - In azure, example of mapping of a block transaction

    ![Flow](ReadmeImages/UI_006.png)

    Some use cases around blocks:
    - In green, most recent block will be rendered at the top. At the moment UI keeps in browser memory info for last 3 blocks received. Example: after running the application, when the forth one is received the oldest(first received) is overriden. 
    - In Yellow, by clicking in a row transaction is possible to see transaction details. Payload is trimmed for memory efficiency and allow fluent UX navigation, at the moment the app is not using a DB, I though it was out of scope for this CA to implement a more refined async solution using redus or a document DB.
    - In Blue, each block can be independently be paginated by buttons up/down or by selecting the specific page from dropdown. 
        - the front end keeps memory of transactions for which details are displayed. As an example if in page 1 I click on row 3 to see transaction details, then I jump to page 3, then jump back on page one, page one will be rendered with details for row 3 expanded. This mechanism works independently between blocks.

    ![Flow](ReadmeImages/UI_007.png)

    It's possible to use multi browser, multi tabs. The web app and in general the net split with back end was implemented on purpose, allow flexibility, use browser memory, and gain from bootstrap out of the box benefits when it comes of resizing, be mobile friendly etc..

    ![Flow](ReadmeImages/UI_008.png)