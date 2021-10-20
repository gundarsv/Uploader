import React from 'react';
import { useHistory } from 'react-router-dom';
import { signinRedirectCallback } from '../services/userService';

function SigninOidc() {
    const history = useHistory();
    React.useEffect(() => {
        async function signinAsync() {
            await signinRedirectCallback().then(
                () => {
                    history.push('/');
                },
                (reason) => {
                    console.log('Rejected', reason);
                },
            );
        }

        (async () => {
            await signinAsync();
        })();
    }, [history]);

    return <div>Redirecting...</div>;
}

export default SigninOidc;
