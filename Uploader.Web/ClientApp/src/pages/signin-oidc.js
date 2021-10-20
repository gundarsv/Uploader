import React, { useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { signinRedirectCallback } from '../services/userService';

function SigninOidc() {
    const history = useHistory();
    useEffect(() => {
        async function signinAsync() {
            await signinRedirectCallback();
            history.push('/');
        }
        signinAsync();
    }, [history]);

    return <div>Redirecting...</div>;
}

export default SigninOidc;
