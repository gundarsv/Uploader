import React from 'react'
import { useSelector } from 'react-redux'
import { Redirect, Route } from 'react-router-dom'

function ProtectedAdministratorRoute({ children, component: Component, ...rest }) {
    const user = useSelector(state => state.auth.user);

    return (user && user.isAdministrator)
        ? (<Route {...rest} component={Component} />)
        : (<Redirect to={'/forbidden'} />)
}

export default ProtectedAdministratorRoute