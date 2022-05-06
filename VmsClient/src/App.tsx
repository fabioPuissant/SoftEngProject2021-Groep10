import React, { useState } from 'react';
import { createBrowserHistory } from 'history';
import { Router, Route, Switch, Redirect } from 'react-router-dom';
import useToken from './custom-hooks/useToken';

// core components
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import Admin from './layouts/Admin';
import './assets/css/material-dashboard-react.css?v=1.6.0';
import LoginView from './views/Login/LoginView';
import { checkRole } from './utils/RoleChecker';

const hist = createBrowserHistory();

const App = () => {
    const { token, userDetail, setToken, } = useToken();

    if (!token) {
        return <LoginView tokenCallback={setToken} />;
    }

    // const [detail] = useState(userDetail ? userDetail : { firstname: 'undefined', roles: [], lastname: 'undefined', username: 'undefined', userref: 'undefined', token: 'undefined' });

    // let redirect = null;
    // if (checkRole(detail.roles != undefined ? detail.roles : [], ["Manager", "Weger", "Voedingsbeheerder"])){
    //     redirect = <Redirect from="/" to="/dashboard/overview" />
    // }
    // else{
    //     redirect = <Redirect from="/" to="/agenda/overview" />
    // }
    
    return (
        <Router history={hist}>
            <Switch>
                <Route path="/dashboard" component={Admin} />
                <Route path="/agenda" component={Admin} />
                <Route path="/login" render={(props: any) => (<LoginView {...props} tokenCallback={setToken} />)} />
                <Route path="/user" component={Admin} />
                <Redirect from="/" to="/dashboard/overview" />
            </Switch>
        </Router>
    );
};

export default App;