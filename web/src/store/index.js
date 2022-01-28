import { createStore } from 'vuex'

export default createStore({
    state: {
        user: null
    },
    mutations: {
        user(state, newUser) {
            state.user = { ...newUser };

            try {
                localStorage.setItem("user", newUser ? JSON.stringify(newUser) : null);
            }
            catch(error) { } // eslint-disable-line
        }
    },
    actions: {
    },
    modules: {
    }
})
