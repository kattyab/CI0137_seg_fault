<script setup lang="ts">
import { computed, onUnmounted, ref, watch } from 'vue'

const props = withDefaults(
  defineProps<{
    images: string[]
    alt: string
    interval?: number
  }>(),
  { interval: 3000 },
)

const index = ref(0)
const current = computed(() => props.images[index.value % props.images.length] ?? '')

let delayTimer: number | undefined
let cycleTimer: number | undefined

function stop() {
  window.clearTimeout(delayTimer)
  window.clearInterval(cycleTimer)
}

function start() {
  if (props.images.length <= 1) return
  if (window.matchMedia('(prefers-reduced-motion: reduce)').matches) return
  // random initial delay so a grid of cards doesn't flip in lockstep
  delayTimer = window.setTimeout(() => {
    cycleTimer = window.setInterval(() => {
      index.value = (index.value + 1) % props.images.length
    }, props.interval)
  }, Math.random() * props.interval)
}

// the image list can grow after mount (e.g. once products load from the API)
watch(
  () => props.images.length,
  () => {
    stop()
    start()
  },
  { immediate: true },
)

onUnmounted(stop)
</script>

<template>
  <div class="cycling-image">
    <Transition name="cycle-fade">
      <img :key="current" :src="current" :alt="alt" />
    </Transition>
  </div>
</template>

<style scoped>
/* Image sizing comes from the surrounding card styles (.card img, .slide img);
   this wrapper only hosts the crossfade. */
.cycling-image {
  position: relative;
  height: 100%;
  overflow: hidden;
}

.cycle-fade-enter-active,
.cycle-fade-leave-active {
  transition: opacity 0.4s ease;
}

.cycle-fade-enter-from,
.cycle-fade-leave-to {
  opacity: 0;
}

/* keep the outgoing image stacked on top so the swap causes no layout shift */
.cycle-fade-leave-active {
  position: absolute;
  inset: 0;
}
</style>
