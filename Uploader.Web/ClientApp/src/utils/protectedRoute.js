import React from 'react'
import { useSelector } from 'react-redux'
import { Redirect, Route } from 'react-router-dom'

function ProtectedRoute({ children, component: Component, ...rest }) {
    const user = useSelector(state => state.auth.user);

    return user
        ? (<Route {...rest} component={Component} />)
        : (<Redirect to={'/login'} />)
}

export default ProtectedRoute