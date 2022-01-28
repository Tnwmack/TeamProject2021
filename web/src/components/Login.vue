<template>
    <a v-if="userName" title="Logout" @click="onNameClick()" class="loginLink">{{userName}}</a>
    <a v-else @click="onLoginClick()" class="loginLink">Login</a>

    <div id="loginBox" class="modal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Login</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <tr><td><input class="form-control" v-model="name" placeholder="User"></td></tr>
                        <tr><td><input type="password" class="form-control" v-model="password" placeholder="Password"></td></tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="onCancelLogin()">Cancel</button>
                    <button type="button" class="btn btn-primary" @click="onConfirmLogin()">Login</button>
                </div>
            </div>
        </div>
    </div>
</template>


<script>
import bootstrap from "bootstrap/dist/js/bootstrap";

export default {
    name: "LoginComponent",
    components: {
    },
    data() {
        return {
            loginBox: null,
            name: "",
            password: ""
        };
    },
    setup() {
    },
    methods: {
        onLoginClick() {
            this.loginBox = new bootstrap.Modal(document.getElementById("loginBox"));
            this.loginBox.show();
        },
        onNameClick() {
            this.$store.commit('user', null);
        },
        onConfirmLogin() {
            let newUser = {
                name: this.name,
                admin: false
            };

            if(this.name === 'admin')
                newUser.admin = true;

            this.$store.commit('user', newUser);

            this.name = "";
            this.password = "";

            this.loginBox.hide();
            this.loginBox = null;
        },
        onCancelLogin() {
            this.name = "";
            this.password = "";

            this.loginBox.hide();
            this.loginBox = null;
        }
    },
    computed: {
        userName() {
            return this.$store.state.user?.name;
        }
    }
};

</script>

<style scoped lang="scss">
    .loginLink {
        cursor: pointer;
    }

</style>