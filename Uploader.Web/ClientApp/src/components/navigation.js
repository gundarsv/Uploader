import React from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { signoutRedirect, signinRedirect } from '../services/userService';

function Navigation() {
    const user = useSelector((state) => state.auth.user);

    function signOut() {
        signoutRedirect();
    }

    function signIn() {
        signinRedirect();
    }

    return (
        <nav className="navigation">
            <section className="container">
                <div className="navigation-title">
                    <h1 className="title">Uploader</h1>
                </div>
                <ul className="navigation-list float-right">
                    <li className="navigation-item">
                        <Link className="navigation-link" to="/">
                            Home
                        </Link>
                    </li>
                    {user && user.isAdministrator ? (
                        <li className="navigation-item">
                            <Link className="navigation-link" to="/settings">
                                Settings
                            </Link>
                        </li>
                    ) : null}
                    {user ? (
                        <li className="navigation-item">
                            <button className="navigation-link button-clear" onClick={() => signOut()}>
                                Sign out
                            </button>
                        </li>
                    ) : (
                        <li className="navigation-item">
                            <button className="navigation-link button-clear" onClick={() => signIn()}>
                                Sign in
                            </button>
                        </li>
                    )}
                </ul>
            </section>
        </nav>
    );
}

export default Navigation;
