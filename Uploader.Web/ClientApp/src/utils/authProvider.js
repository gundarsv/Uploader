import React from 'react';
import { useDispatch } from 'react-redux';
import { removeUser, storeUser } from '../slices/authSlice';
import { setAuthHeader } from './axiosHeaders';

export default function AuthProvider({ userManager: manager, store, children }) {
    let userManager = React.useRef();

    const dispatch = useDispatch();

    React.useEffect(() => {
        userManager.current = manager;

        const onUserLoaded = (user) => {
            setAuthHeader(user.access_token);
            dispatch(storeUser({ name: user.profile.name, isAdministrator: user.profile.role === 'Administrator' }));
        };

        const onUserUnloaded = () => {
            setAuthHeader(null);
        };

        const onAccessTokenExpiring = () => {
            setAuthHeader(null);
            dispatch(removeUser());
        };

        const onAccessTokenExpired = () => {
            setAuthHeader(null);
            dispatch(removeUser());
        };

        const onUserSignedOut = () => {
            setAuthHeader(null);
            dispatch(removeUser());
        };

        // events for user
        userManager.current.events.addUserLoaded(onUserLoaded);
        userManager.current.events.addUserUnloaded(onUserUnloaded);
        userManager.current.events.addAccessTokenExpiring(onAccessTokenExpiring);
        userManager.current.events.addAccessTokenExpired(onAccessTokenExpired);
        userManager.current.events.addUserSignedOut(onUserSignedOut);

        // Specify how to clean up after this effect:
        return function cleanup() {
            userManager.current.events.removeUserLoaded(onUserLoaded);
            userManager.current.events.removeUserUnloaded(onUserUnloaded);
            userManager.current.events.removeAccessTokenExpiring(onAccessTokenExpiring);
            userManager.current.events.removeAccessTokenExpired(onAccessTokenExpired);
            userManager.current.events.removeUserSignedOut(onUserSignedOut);
        };
    }, [manager, store, dispatch]);

    return React.Children.only(children);
}
