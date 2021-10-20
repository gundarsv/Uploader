import React from 'react';
import { useHistory } from 'react-router-dom';
import { signoutRedirectCallback } from '../services/userService';

function SignoutOidc() {
    const history = useHistory();
    React.useEffect(() => {
        async function signoutAsync() {
            await signoutRedirectCallback().then(
                () => {
                    history.push('/');
                },
                (reason) => {
                    console.log('Rejected', reason);
                },
            );
        }

        (async () => {
            await signoutAsync();
        })();
    }, [history]);

    return <div>Redirecting...</div>;
}

export default SignoutOidc;
