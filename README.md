# Poloniex Auto Trader

---------------------------------------

A WPF dashboard for running automated trading strategies on Poloniex Exchange.
This is tested and working, but some things have been done to correct async methods from getting incorrect nonce from API.

### Usage
** Dummy API key loaded to get app running **
1. Open API key entry window, enter you own API keys from Poloniex
2. Save keys and restart app

![API Keys](https://github.com/ColossusFX/PoloniexAutoTrader/blob/master/Screenshot_1.jpg?raw=true "API Key Entry")

3. Select Market Period, Currency Pair and trade size in BTC
4. Click start
** Each algo is then added to the datagrid **

![Start Algo](https://github.com/ColossusFX/PoloniexAutoTrader/blob/master/Screenshot_3.jpg?raw=true "Start Algo")

5. To stop an algo, click on the list and press the stop button.

![Stop Algo](https://github.com/ColossusFX/PoloniexAutoTrader/blob/master/Screenshot_4.jpg?raw=true "Stop Algo")

## Strategies
- 5 Built in strategies
- Dispatcher timer for running strategies at Market Period intervals
- Output data to window

### Features
- Tick data working
- All functions for data working
 - Some changes need to be made for some sync methods
 
 #### To Do
- Some changes have been made which need testing
- Moving average cross over strategy to upload

#### Dependencies
Make sure to download, build and reference my fork of PoloniexApi.Net from my repo.

## Contribute
Check out the [contribution guidelines](CONTRIBUTING.md)
if you want to contribute to this project.

## License
[Apache 2.0](LICENSE)
