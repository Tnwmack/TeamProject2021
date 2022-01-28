<template>
    <div>
        <div v-if="employees == null" class="loadingGroup">
            <table class="table">
                <thead>
                    <tr>
                        <th><button v-if="user?.admin" type="button" class="btn btn-m btn-primary py-1" title="Add" @click="onAdd()">Add</button></th>
                        <th><span>First Name</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th><span>Last Name</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th><span>Title</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th><span>ID</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th><span>Tenure</span><button type="button" class="btn"><i class="gg-arrows-v"></i></button></th>
                        <th v-if="user?.admin"><span>Active</span></th>
                        <th v-if="user?.admin"></th>
                    </tr>
                </thead>

                <tbody>
                    <tr v-for="n in 10" :key="n">
                        <td><div class="loadingPicture loadingAnimate"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 10em"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 10em"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 10em"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 2em"></div></td>
                        <td><div class="loadingText loadingAnimate" style="width: 2em"></div></td>
                        <td v-if="user?.admin"><div class="loadingText loadingAnimate" style="width: 2em"></div></td>
                        <td v-if="user?.admin"><div class="loadingPicture loadingAnimate"></div></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <table v-else class="table">
            <thead>
                <tr>
                    <th><button v-if="user?.admin" type="button" class="btn btn-m btn-primary py-1" title="Add" @click="onAdd()">Add</button></th>
                    <th><span>First Name</span><button type="button" class="btn" @click="onSortClicked('firstname')"><i class="gg-arrows-v"></i></button></th>
                    <th><span>Last Name</span><button type="button" class="btn" @click="onSortClicked('lastname')"><i class="gg-arrows-v"></i></button></th>
                    <th style="min-width: 120px"><span>Title</span><button type="button" class="btn" @click="onSortClicked('title')"><i class="gg-arrows-v"></i></button></th>
                    <th style="min-width: 70px"><span>ID</span><button type="button" class="btn" @click="onSortClicked('id')"><i class="gg-arrows-v"></i></button></th>
                    <th style="min-width: 70px"><span>Tenure</span><button type="button" class="btn" @click="onSortClicked('hiredate')"><i class="gg-arrows-v"></i></button></th>
                    <th v-if="user?.admin"><span>Active</span></th>
                    <th v-if="user?.admin"></th>
                </tr>
            </thead>

            <tbody>
                <tr v-for="employee in employees" :key="employee.id">
                    <td class="clickable" @click="onEmployee(employee.id)"><img :src="gravatar(employee.email)" height="70" class="d-inline-block"></td>
                    <td class="clickable" @click="onEmployee(employee.id)">{{employee.firstName}}</td>
                    <td class="clickable" @click="onEmployee(employee.id)">{{employee.lastName}}</td>
                    <td class="clickable" @click="onEmployee(employee.id)">{{employee.title}}</td>
                    <td class="clickable" @click="onEmployee(employee.id)">{{employee.id}}</td>
                    <td class="clickable" @click="onEmployee(employee.id)">{{tenure(employee.hireDate)}}</td>
                    <td v-if="user?.admin" class="clickable" @click="onEmployee(employee.id)">{{employee.active === true ? "yes" : "no"}}</td>
                    <td v-if="user?.admin">
                        <button v-if="employee.active === true" type="button" class="btn" title="Deactivate" @click="onActivation(employee.id, false)"><i class="gg-close-r deactivate-button"></i></button>
                        <button v-else type="button" class="btn" title="Activate" @click="onActivation(employee.id, true)"><i class="gg-close-r activate-button"></i></button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import md5 from "md5";
export default {
    name: "EmployeeList",
    props: {
        employees: Array
    },
    methods: {
        onActivation(id, activate) {this.$emit('onActivation', id, activate);},
        onAdd() {this.$emit('onAdd');},
        onSortClicked(column) {this.$emit('onSort', column);},
        onEmployee(id) {this.$router.push({ name: 'EmployeeDetails', params: { id } });},
        gravatar(email) {
            if (email) {
                const hash = md5(email.trim().toLowerCase());
                return `https://www.gravatar.com/avatar/${hash}?d=mp`;
            }
            return '/employees/na_employee.png'
        },
        tenure(hireDate) {
            let tenureMillis = Date.now() - Date.parse(hireDate); //Date.now() - hireDate;
            let tenureAsDate = new Date(tenureMillis); // as in, miliseconds from epoch
            let tenureInYears = Math.abs(tenureAsDate.getFullYear() - 1970);
            return (tenureInYears < 1? '<1' : tenureInYears);
        }
    },
    computed: {
        user() {
            return this.$store.state.user;
        }
    },
    emits: ['onActivation', 'onEmployee', 'onSort', 'onAdd']
};
</script>

<style scoped lang="scss">
    @import "../../node_modules/css.gg/icons/scss/arrows-v.scss";
    @import "../../node_modules/css.gg/icons/scss/close-r.scss";
    @import "../../node_modules/css.gg/icons/scss/check-r.scss";
    @import "../styles/loading.scss";

    .deactivate-button {
        background-color: orangered;
    }

    .activate-button {
        background-color: greenyellow;
    }

    .clickable {
        cursor: pointer;
    }

    .searchBox {
        display: block;
    }
</style>