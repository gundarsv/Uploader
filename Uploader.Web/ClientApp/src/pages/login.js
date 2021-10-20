import React from 'react'
import { useSelector } from 'react-redux'
import { Redirect } from 'react-router-dom'
import Navigation from '../components/navigation'

function Login() {
    const user = useSelector(state => state.auth.user)

    return (
        (user) ?
            (<Redirect to={'/'} />)
            :
            (
                <>
                    <Navigation />
                    <div style={{ 'textAlign': 'center' }} className="container">
                        <h1>Hello!</h1>
                        <p>Start by signing in.</p>
                        <p>
                            <em>User: 'bob', Pass: 'bob'</em>
                        </p>
                        <p>
                            <em>Administrator: 'alice', Pass: 'alice'</em>
                        </p>
                    </div>
                </>
            )
    )
}

export default Login
