import React, { useEffect, useState } from 'react';
import { Provider } from 'react-redux';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Forbidden from './pages/forbidden';
import Home from './pages/home';
import Login from './pages/login';
import Settings from './pages/settings';
import SigninOidc from './pages/signin-oidc';
import SignoutOidc from './pages/signout-oidc';
import userManager, { loadUserFromStorage } from './services/userService';
import store from './store';
import AuthProvider from './utils/authProvider';
import ProtectedAdministratorRoute from './utils/protectedAdministratorRoute';
import ProtectedRoute from './utils/protectedRoute';
import Loader from 'react-loader-spinner';
import Upload from './pages/upload';
import CreateSettings from './pages/createSettings';

function App() {
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        (async () => {
            await loadUserFromStorage(store);
            setTimeout(function () {
                setIsLoading(false);
            }, 1000);
        })();
    }, []);

    return isLoading ? (
        <div style={{ textAlign: 'center', marginTop: '20%' }}>
            <Loader type="TailSpin" color="#9b4dca" height={100} width={100} />
        </div>
    ) : (
        <Provider store={store}>
            <AuthProvider userManager={userManager} store={store}>
                <Router>
                    <Switch>
                        <Route path="/forbidden" component={Forbidden} />
                        <Route path="/login" component={Login} />
                        <Route path="/signout-oidc" component={SignoutOidc} />
                        <Route path="/signin-oidc" component={SigninOidc} />
                        <ProtectedRoute exact path="/" component={Home} />
                        <ProtectedAdministratorRoute exact path="/settings" component={Settings} />
                        <ProtectedAdministratorRoute exact path="/upload" component={Upload} />
                        <ProtectedAdministratorRoute exact path="/createSettings" component={CreateSettings} />
                    </Switch>
                </Router>
            </AuthProvider>
        </Provider>
    );
}

export default App;
