<template>
    <div id="modalBox" class="modal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">{{text.title}}</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    {{text.message}}
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="onCancel()">{{text.cancel}}</button>
                    <button type="button" class="btn btn-primary" @click="onAccept()">{{text.accept}}</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import bootstrap from "bootstrap/dist/js/bootstrap";

export default {
    name: "ConfirmationBox",
    props: {
        text: Object
    },
    data() {
        return {
            box: null,
            manualClose: false
        };
    },
    mounted() {
        let data = this;
        let boxDom = document.getElementById("modalBox");
        this.box = new bootstrap.Modal(boxDom);

        boxDom.addEventListener('hidden.bs.modal', function () {
            data.onModalClosed();
        });

        this.box.show();
    },
    methods: {
        onAccept() {
            this.manualClose = true;
            this.box.hide();
            this.$emit('onClose', true);
        },
        onCancel() {
            this.manualClose = true;
            this.box.hide();
            this.$emit('onClose', false);
        },
        onModalClosed() {
            if(this.manualClose === false)
                this.$emit('onClose', false);
        },
    },
    emits: ['onClose']
};
</script>

<style scoped lang="scss">


</style>