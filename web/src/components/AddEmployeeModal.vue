<template>
    <div id="modalBox" class="modal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">{{employee ? "Edit an employee" : "Add an employee"}}</h5>
                </div>
                <div class="modal-body">
                    <table class="table">
                        <tr><td style="text-align: right;">First Name:</td><td><input class="form-control" v-model="firstName" placeholder="enter first name"></td></tr>
                        <tr v-if="firstNameError"><td></td><td><div class="alert alert-danger table-danger">First name required</div></td></tr>
                        <tr><td style="text-align: right;">Last Name:</td><td><input class="form-control" v-model="lastName" placeholder="enter last name"></td></tr>
                        <tr v-if="lastNameError"><td></td><td><div class="alert alert-danger table-danger">Last name required</div></td></tr>
                        <tr><td style="text-align: right;">Title:</td><td><input class="form-control" v-model="jobTitle" placeholder="enter title"></td></tr>
                        <tr v-if="jobTitleError"><td></td><td><div class="alert alert-danger table-danger">Job title required</div></td></tr>
                        <tr><td style="text-align: right;">Email:</td><td><input class="form-control" v-model="email" placeholder="enter email"></td></tr>
                        <tr><td style="text-align: right;">Hire Date:</td><td><input class="form-control" type="date" id="hiredate" name="hiredate" v-model="hireDate"></td></tr>
                        <tr v-if="hireDateError"><td></td><td><div class="alert alert-danger table-danger">Hire date required</div></td></tr>
                        <tr><td style="text-align: right;">Bio:</td><td><textarea class="form-control" v-model="bio" placeholder="enter bio"></textarea></td></tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="onCloseCancel()">Cancel</button>

                    <button v-if="showWorkingButton" type="button" class="btn btn-warning btn-working">Sending...</button>
                    <button v-else type="button" class="btn btn-primary" @click="onAccept()">Accept</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import bootstrap from "bootstrap/dist/js/bootstrap";
import { useToast } from "vue-toastification";
import CorpAPI from "@/api";

export default {
    name: "AddEmployeeModal",
    data() {
        return {
            showWorkingButton: false,
            box: null,
            firstName: "",
            lastName: "",
            jobTitle: "",
            email: "",
            hireDate: new Date().toISOString().substr(0, 10),
            bio: "",
            firstNameError: false,
            lastNameError: false,
            jobTitleError: false,
            hireDateError: false,
            manualClose: false,
        };
    },
    props: {
        employee: Object
    },
    setup() {
        const toast = useToast();
        return { toast };
    },
    mounted() {
         let data = this;

        if(this.employee) {
            this.firstName = this.employee.firstName || "";
            this.lastName = this.employee.lastName || "";
            this.jobTitle = this.employee.title || "";
            this.email = this.employee.email || "";
            this.hireDate = this.employee.hireDate.substr(0, 10);
            this.bio = this.employee.bio || "";
        }

        let boxDom = document.getElementById("modalBox");
        this.box = new bootstrap.Modal(boxDom);

        boxDom.addEventListener('hidden.bs.modal', function () {
            data.onModalClosed();
        });

        this.box.show();
    },
    methods: {
        validateForm() {
            let validated = true;

            if(this.firstName.trim().length === 0) {
                validated = false;
                this.firstNameError = true;
            } else {
                this.firstNameError = false;
            }

            if(this.lastName.trim().length === 0) {
                validated = false;
                this.lastNameError = true;
            } else {
                this.lastNameError = false;
            }

            if(this.jobTitle.trim().length === 0) {
                validated = false;
                this.jobTitleError = true;
            } else {
                this.jobTitleError = false;
            }

            let date = new Date(this.hireDate);

            if(date.getTime() !== date.getTime()) {
                validated = false;
                this.hireDateError = true;
            } else {
                this.hireDateError = false;
            }

            return validated;
        },
        async onAccept() {
            if(this.validateForm()) {
                this.showWorkingButton = true;

                if(await this.submitEmployee()) {
                    this.toast.success(this.employee ? "Employee updated" : "Employee added");
                    this.onCloseSuccess();
                }
                else {
                    this.toast.error(this.employee ? "Update employee failed" : "Add employee failed", {timeout: false});
                }

                this.showWorkingButton = false;
            }
        },
        onCloseCancel() {
            this.manualClose = true;
            this.box.hide();
            this.$emit('onClose', false);
        },
        onCloseSuccess() {
            this.manualClose = true;
            this.box.hide();
            this.$emit('onClose', true);
        },
        onModalClosed() {
            if(this.manualClose === false)
                this.$emit('onClose', false);
        },
        async submitEmployee() {
            const newEmployee = {
                'firstName': this.firstName,
                'lastName': this.lastName,
                'title': this.jobTitle,
                'hireDate': this.hireDate,
                'email': this.email.trim().length === 0 ? undefined : this.email,
                'bio': this.bio.trim().length === 0 ? undefined : this.bio
            };

            if(this.employee) {
                newEmployee.id = this.employee.id;
                return await CorpAPI.updateEmployee(newEmployee);
            }
            else {
                return await CorpAPI.createEmployee(newEmployee);
            }
        }
    },
    emits: ['onClose']
};
</script>

<style scoped lang="scss">
    .btn-working {
        cursor: progress;
    }

</style>