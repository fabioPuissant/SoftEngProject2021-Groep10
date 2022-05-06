import React from 'react';
import ReactDOM from 'react-dom';
import { createBrowserHistory } from 'history';
import './assets/css/material-dashboard-react.css?v=1.6.0';
import App from './App';

const hist = createBrowserHistory();

ReactDOM.render(
  // tslint:disable-next-line: jsx-wrap-multiline
  <App />,
  document.getElementById('root')
);
